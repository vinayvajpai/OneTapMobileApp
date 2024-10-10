using OneTapMobile.LocalDataBases;
using OneTapMobile.LocalDataBases.DataBaseModel;
using OneTapMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.LocalDataBase
{
   public class InitDatabaseTable : DataBase
    {
        protected override void InitDatabaseTables()
        {
            Connection.CreateTable<LoginUserDataDBModel>();
            Connection.CreateTable<ProfileDBModel>();
            Connection.CreateTable<facebookProfileDBModel>();
            Connection.CreateTable<GoRefreshTokenDBModel>();
            Connection.CreateTable<FBcustomerAccListDBModel>();
            Connection.CreateTable<GoogleAdsCustomersDBModel>();
            Connection.CreateTable<FacebookPageIdDBModel>();
            Connection.CreateTable<SelectedGoAdCustDetailDBModel>();
            Connection.CreateTable<TokenExpireTimeDBModel>();
            Connection.CreateTable<ImageCampaignModel>();
            Connection.CreateTable<VideoCampaignModel>();
            Connection.CreateTable<KeywordCampaignModel>();
        }
    }
}
