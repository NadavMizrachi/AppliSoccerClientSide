using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AppliSoccerClientSide.ViewModel
{
    public class OrderMetadataViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string SenderName { get; set; }

        private DateTime _sentDate;
        public DateTime SentDate
        {
            get { return _sentDate.ToLocalTime(); }
            set
            {
                _sentDate = TimeZoneInfo.ConvertTimeToUtc(value);
                OnPropertyChanged("SentDate");
            }
        }


        private bool _wasRead;
        public bool WasRead
        {
            get { return _wasRead; }
            set
            {
                if (value == _wasRead) return;
                _wasRead = value;
                if (!_wasRead)
                    FontAttribute = FontAttributes.Bold;
                else
                    FontAttribute = FontAttributes.None;
                OnPropertyChanged("WasRead");
            }
        }

        private FontAttributes _fontAttributes = FontAttributes.Bold;
        public FontAttributes FontAttribute
        {
            get { return _fontAttributes; }
            set
            {
                if (value == _fontAttributes) return;
                _fontAttributes = value;
                OnPropertyChanged("FontAttribute");
            }
        }
        //public FontAttributes FontAttribute

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static OrderMetadataViewModel Convert(OrderMetadata omd)
        {
            return new OrderMetadataViewModel()
            {
                Id = omd.OrderId,
                SenderName = omd.SenderName,
                SentDate = omd.SentDate,
                Title = omd.Title,
                WasRead = omd.WasRead
            };
        }

        public static List<OrderMetadataViewModel> ConvertList(List<OrderMetadata> orderMetadatas)
        {
            return orderMetadatas
                .ConvertAll(omd => OrderMetadataViewModel.Convert(omd))
                .ToList();
        }
    }
}
