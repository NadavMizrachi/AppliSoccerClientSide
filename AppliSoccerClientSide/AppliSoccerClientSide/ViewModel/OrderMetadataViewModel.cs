﻿using AppliSoccerObjects.Modeling;
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
        #region Title
        public string Title { get; set; }
        #endregion

        #region ID
        public string Id { get; set; }
        #endregion

        #region SenderName
        public string SenderName { get; set; }
        #endregion

        #region SendDate
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
        #endregion

        #region WasRead
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
        #endregion

        #region FontAttribute
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
        #endregion

        #region ReceiversNames
        public List<string> ReceiversNames { get; set; }
        #endregion

        #region ReceiverInfos
        public List<ReceiverInfo> RecieverInfos { get; set; } 
        #endregion

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
                WasRead = omd.WasRead,
                ReceiversNames = omd.ReceiversNames
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
