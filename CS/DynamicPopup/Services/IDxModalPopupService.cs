using DynamicPopup.Models;
using Microsoft.AspNetCore.Components;

namespace DynamicPopup.Services {
    public interface IDxModalPopupService {
        public Task CloseModal(DxPopupModel modal);
        public Task ShowModal<TComponent>(Dictionary<string, object>? attributes = null) where TComponent : ComponentBase;
    }
}
