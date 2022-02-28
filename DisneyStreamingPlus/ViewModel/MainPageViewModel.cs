using DisneyStreamingPlus.Model;
using System.Collections.ObjectModel;

namespace DisneyStreamingPlus.ViewModel
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            ImagesGrouped = new ObservableCollection<GroupInfoList>(StreamingCatalog.GetImagesGroupedAsync().Result);
        }

        public ObservableCollection<GroupInfoList> ImagesGrouped { get; set; }
    }
}