using System;

namespace Assets.Scripts
{
    public class EventAggregatorForTutorial
    {
        public static event EventHandler GuySelected;

        public static void PublishGuySelected()
        {
            GuySelected?.Invoke(null, EventArgs.Empty);
        }
    }
}
