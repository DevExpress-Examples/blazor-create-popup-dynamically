<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1236052)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Blazor Popup - Add Content Dynamically

This example creates a service to generate the Popup's content dynamically.

![Add Popup content in code](/blazor-popup-created-dynamically.png)

Follow the steps below to implement this functionality:

1. Create a service that shows and closes the Popup component. This service allows you to declare the `DxPopup` component in your application only once. Refer to the following folder for full service implementation: [Services](/CS/DynamicPopup/Services/).

1. Register the service in the _Program.cs_ file:

    ```cs
    builder.Services.AddScoped<IDxModalPopupService, DxModalPopupService>();
    ```

1. Create a Popup model based on which the custom pop-up's content is generated. Refer to the following file for full implementation: [DxPopupModel.cs](/CS/DynamicPopup/Models/DxPopupModel.cs).

1. Create a Razor component ([DxModalPopup.razor](/CS/DynamicPopup/Components/DxModalPopup.razor)) that is connected to the service:

    ```Razor
    @inject IDxModalPopupService modalService

    <DxPopup></DxPopup>

    <DxPopup @key="@model" ShowCloseButton="true" Closed="() => OnClosed(model)" Visible="true">
        <BodyContentTemplate>
            <CascadingValue Value="@model" IsFixed="true">
                <DynamicComponent Type="@model.ComponentType" Parameters="@model.Attributes" />
            </CascadingValue>
        </BodyContentTemplate>
    </DxPopup>

    @code {
        DxModalPopupService DxModalPopupService {
            get {
                return (DxModalPopupService)modalService;
            }
        }

        protected override void OnInitialized() {
            base.OnInitialized();
            DxModalPopupService.ShownAsync += ModalShown;
            DxModalPopupService.ClosedAsync += ModalClosed;
        }

        protected void OnClosed(DxPopupModel model) {        
            DxModalPopupService.Result(model);
        }

        protected async Task ModalShown(DxPopupModel model) {
            await InvokeAsync(StateHasChanged);
        }

        protected async Task ModalClosed(DxPopupModel model) {
            OnClosed(model);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose() {
            DxModalPopupService.ShownAsync -= ModalShown;
            DxModalPopupService.ClosedAsync -= ModalClosed;
        }
    }
    ```

1. _Optional._ If you want to show several pop-up windows on one page, create several `<DxPopup>` instances. To do so, wrap the Popup's markup in the `@foreach` loop:

    ```Razor
    @foreach (var model in DxModalPopupService.Modals) {
        <DxPopup @key="@model" ShowCloseButton="true" Closed="() => OnClosed(model)" Visible="true">
            @* ... *@
        </DxPopup>
    }
    ```

1. Declare the `DxModalPopup` component in the _App.razor_ file. This is the only declaration of the component in the application.

    ```Razor
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <DxModalPopup />
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
        </Found>
    </Router>
    ```

1. Create a `.cs` file that implements content generation and show and close operations. Refer to the following file for the full implementation: [DxModalPopup.cs](/CS/DynamicPopup/Components/DxModalPopup.cs).

1. Create a Razor file ([ComponentWithCloseButton.razor](/CS/DynamicPopup/Components/ComponentWithCloseButton.razor)) that accepts parameters and uses the model to render content:

    ```Razor
    @inject IDxModalPopupService Modal

    <h3>A Component With Close Button</h3>

    <p>Passed value: @DemoText</p>

    <DxButton Click="async () => await Modal!.CloseModal(PopupModel)">Close</DxButton>

    @code {
        [CascadingParameter]
        DxPopupModel PopupModel { get; set; }

        [Parameter]
        public string DemoText { get; set; } = String.Empty;
    }
    ```

1. In the [Index.razor](/CS/DynamicPopup/Pages/Index.razor) file, create a button that opens your custom pop-up through a service and pass parameters:

    ```Razor
    @inject IDxModalPopupService Modal

    <h2 class="pt-2">@text</h2>

    <DxButton Text="Show a modal with parameter"
              Click="@(async () => {
                  text = "Modal with parameter button is opened!";
                  await Modal.ShowModal<ComponentWithCloseButton>(new() {
                      { nameof(ComponentWithCloseButton.DemoText), "Modal Content" }
                  });
                  text = "Modal with parameter was closed!";
              })">
    </DxButton>

    @code {
        string text = "Modal Popup";
    }
    ```

## Files to Review

- [DxModalService.cs](/CS/DynamicPopup/Services/DxModalPopupService.cs)
- [DxModalPopup.razor](/CS/DynamicPopup/Components/DxModalPopup.razor)
- [DxModalPopup.cs](/CS/DynamicPopup/Components/DxModalPopup.cs)
- [ComponentWithCloseButton.razor](/CS/DynamicPopup/Components/ComponentWithCloseButton.razor)

## Documentation

- [DxPopup](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxPopup)

## More Examples

- [Popup for Blazor - How to implement a confirmation dialog](https://github.com/DevExpress-Examples/blazor-popup-confirmation-dialog)
