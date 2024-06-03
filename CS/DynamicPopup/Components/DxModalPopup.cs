using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;

namespace DynamicPopup.Components {
    public partial class DxModalPopup : DxPopup {
        TaskCompletionSource? popupCompletion;

        [Parameter]
        public RenderFragment BodyContent { get; set; } = new(builder => { });

        protected void CreateBodyContent(Type? componentType, Dictionary<string, object>? attributes) {
            BodyContent = new RenderFragment(builder => {
                if (componentType != null) {
                    int sequence = 0;
                    builder.OpenComponent<CascadingValue<DxModalPopup>>(sequence++);
                    builder.AddMultipleAttributes(sequence++, new Dictionary<string, object>() {
                        { nameof(CascadingValue<DxModalPopup>.Value), this },
                        { nameof(CascadingValue<DxModalPopup>.IsFixed), true },
                        { nameof(CascadingValue<DxModalPopup>.ChildContent), new RenderFragment(childBuilder => {
                            childBuilder.OpenComponent(0, componentType);
                            if (attributes != null) {
                                childBuilder.AddMultipleAttributes(1, attributes);
                            }
                            childBuilder.CloseComponent();
                        })}
                    });
                    builder.CloseComponent();
                }
            });
        }

        public DxModalPopup() {
            this.BodyContentTemplate = new RenderFragment<IPopupElementInfo>(target => BodyContent);
            this.Closed = EventCallback.Factory.Create<PopupClosedEventArgs>(this, (e) => {
                OnClosed(e);
            });
        }

        protected void OnClosed(PopupClosedEventArgs e) {
            popupCompletion?.TrySetResult();
        }

        public async Task ShowModal<TComponent>(Dictionary<string, object>? attributes = null) where TComponent : ComponentBase {
            CreateBodyContent(typeof(TComponent), attributes);
            popupCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            await modalService.ShowModal<TComponent>(attributes);           
            await popupCompletion.Task;
        }
    }
}
