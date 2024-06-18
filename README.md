<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1236052)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Blazor Popup - Add Content Dynamically

This example creates a service to generate the Popup's content dynamically.

![Add Popup content in code](/blazor-popup-created-dynamically.png)

Follow the steps below to implement this functionality:

1. Create a service that shows and closes the pop-up window. This service allows you to declare the `DxPopup` component in your application only once. Refer to the following folder for the full service implementation: [Services](/CS/DynamicPopup/Services/).

1. Register the service in the _Program.cs_ file:

    ```cs
    builder.Services.AddScoped<IDxModalPopupService, DxModalPopupService>();
    ```

1. Create a Popup model based on which the custom popup's content is generated. Refer to the following file for the full implementation: [DxPopupModel.cs](/CS/DynamicPopup/Models/DxPopupModel.cs).

1. Create a Razor component ([DxModalPopup.razor](/CS/DynamicPopup/Components/DxModalPopup.razor)) that injects the service.

1. _Optional._ If you want to show multiple pop-up windows on one page, create the corresponding number of `DxPopup` instances. Wrap the Popup's markup in a `@foreach` loop:

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

1. Create a `.cs` file that implements content generation, and show and close operations. Refer to the following file for the full implementation: [DxModalPopup.cs](/CS/DynamicPopup/Components/DxModalPopup.cs).

1. Create a Razor file ([ComponentWithCloseButton.razor](/CS/DynamicPopup/Components/ComponentWithCloseButton.razor)) that accepts parameters and uses the model to render content:

    ```Razor
    @inject IDxModalPopupService Modal

    <h3>A Component with a Close Button</h3>

    <p>Passed value: @DemoText</p>

    <DxButton Click="async () => await Modal!.CloseModal(PopupModel)">Close</DxButton>

    @code {
        [CascadingParameter]
        DxPopupModel PopupModel { get; set; }

        [Parameter]
        public string DemoText { get; set; } = String.Empty;
    }
    ```

1. In the [Index.razor](/CS/DynamicPopup/Pages/Index.razor) file, create a button that opens your custom pop-up through a service and pass corresponding parameters to the button's `Click` event handler:

    ```Razor
    @inject IDxModalPopupService Modal

    <h2 class="pt-2">@text</h2>

    <DxButton Text="Show modal with parameter"
              Click="@(async () => {
                  text = "Modal with parameter button is open!";
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
