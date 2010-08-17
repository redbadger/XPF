namespace RedBadger.Xpf.Presentation.Input
{
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class InputManager
    {
        private readonly IMouse mouse;

        private readonly IElement rootElement;

        public InputManager(IElement rootElement, IMouse mouse)
        {
            this.rootElement = rootElement;
            this.mouse = mouse;

            this.mouse.MouseData.Subscribe(Observer.Create<MouseData>(this.OnNextMouseData));
        }

        public void Update()
        {
            this.mouse.Update();
        }

        private void OnNextMouseData(MouseData mouseData)
        {
            this.rootElement.OnNextMouseData(mouseData);
        }
    }
}