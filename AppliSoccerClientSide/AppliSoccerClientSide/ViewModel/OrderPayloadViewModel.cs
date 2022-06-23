using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class OrderPayloadViewModel : INotifyPropertyChanged
    {

        private string _senderName;
        public string SenderName 
        {
            get { return _senderName; }
            set
            {
                _senderName = value;
                OnPropertyChanged("SenderName");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                var str = value;
                _content = str.Replace("\r", Environment.NewLine).Replace("\n", Environment.NewLine);
                OnPropertyChanged("Content");
            }
        }

        private List<string> _receivers;
        public List<string> Receivers
        {
            get { return _receivers; }
            set
            {
                _receivers = value;
                OnPropertyChanged("Receivers");
            }
        }

        private DateTime _sendingDate;
        public DateTime SendingDate
        {
            get { return _sendingDate.ToLocalTime(); }
            set
            {
                _sendingDate = TimeZoneInfo.ConvertTimeToUtc(value);
                OnPropertyChanged("SendingDate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public static OrderPayloadViewModel ConvertFromOrderPayload(OrderPayload orderPayload)
        {
            return new OrderPayloadViewModel
            {
                SenderName = orderPayload.SenderName,
                Content = orderPayload.Content,
                Title = orderPayload.Title,
                Receivers = orderPayload.ReceiversNames,
                SendingDate = orderPayload.SendingDate
            };
        }
    }
}
