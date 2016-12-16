using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TwitterService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITwitter" in both code and config file together.
    [ServiceContract]
    public interface ITwitter
    {
        [OperationContract]
        int Register(string email, string username, string password, string name);

        [OperationContract]
        int Login(string user, string password);

        [OperationContract]
        bool Update(int id,string email, string username, string password, string name);

        [OperationContract]
        bool Follow(int follower, int followed);

        [OperationContract]
        int NewTwitControl(int id, DateTime lastDate);

        [OperationContract]
        List<TwitLocal> GetNewTwits(int id, DateTime lastDate);

        [OperationContract]
        bool AddNewTwit(int id, string content);

        [OperationContract]
        bool TwitFav(int id, int twitId);

        [OperationContract]
        List<TwitLocal> GetAllTwits(int id);

        [OperationContract]
        KisiLocal GetUser(int id);

        [OperationContract]
        KisiLocal Search(string contains);
    }
}
