using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Neighbor.Mobile.UITest.Scenes.User
{
    public class TermAndConditionScene
    {
        private IApp app;

        public AppQuery TermAndConditionScroll(AppQuery c) => c.Marked("TermAndConditionScroll");
        public AppQuery TermAndConditionAcceptButton(AppQuery c) => c.Marked("TermAndConditionAcceptButton");
        public AppQuery CancelAcceptTCButton(AppQuery c) => c.Marked("CancelAcceptTCButton");


        public TermAndConditionScene(IApp app)
        {
            this.app = app;
        }

        public void Play()
        {
            app.ScrollDown(TermAndConditionScroll, ScrollStrategy.Gesture, 0.9, 2000);
            app.ScrollDown(TermAndConditionScroll, ScrollStrategy.Gesture, 0.9, 2000);
            app.ScrollDown(TermAndConditionScroll, ScrollStrategy.Gesture, 0.9, 2000);
            app.ScrollDown(TermAndConditionScroll, ScrollStrategy.Gesture, 0.9, 2000);           

            app.Tap(TermAndConditionAcceptButton);
        }
    }
}
