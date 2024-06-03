using DynamicPopup.Models;
using Microsoft.AspNetCore.Components;

namespace DynamicPopup.Services {
    public class DxModalPopupService : IDxModalPopupService {
        public event Func<DxPopupModel, Task>? ShownAsync;
        public event Func<DxPopupModel, Task>? ClosedAsync;

        public List<DxPopupModel> Modals { get; private set; } = new List<DxPopupModel>();

        public void Result(DxPopupModel popup) {
            popup.Result();
            Modals.Remove(popup);
        }

        public async Task CloseModal(DxPopupModel popup) {
            if (ClosedAsync != null) {
                await ClosedAsync(popup);
            }
        }

        public async Task ShowModal<TComponent>(Dictionary<string, object>? attributes = null) where TComponent : ComponentBase {
            if (ShownAsync != null) {
                DxPopupModel model = new DxPopupModel(typeof(TComponent), attributes);
                Modals.Add(model);
                await ShownAsync(model);
                await model.Task;
            }
        }
    }
}
