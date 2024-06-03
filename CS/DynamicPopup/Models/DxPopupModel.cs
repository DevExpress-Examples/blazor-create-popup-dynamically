using DevExpress.Blazor;

namespace DynamicPopup.Models {
    public class DxPopupModel {
        TaskCompletionSource popupCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        public Type ComponentType { get; set; }
        public Dictionary<string, object>? Attributes { get; set; }

        public Task Task {
            get { return popupCompletion.Task; }
        }

        public DxPopupModel(Type componentType, Dictionary<string, object>? attributes) {
            ComponentType = componentType;
            Attributes = attributes;
        }

        public void Result() {
            popupCompletion.TrySetResult();
        }
    }
}
