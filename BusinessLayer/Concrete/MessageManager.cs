using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class MessageManager: IMessageService
    {
        IMessageDal  _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public Message GetByID(int id)
        {
            return _messageDal.Get(x=>x.MessageID == id);
        }

        public List<Message> GetListInbox(string p)
        {
            return _messageDal.List(x => x.ReceiverMail == p && x.MessageDeleted == false);
        }

        public List<Message> GetListSendbox(string p)
        {
            return _messageDal.List(x => x.SenderMail == p && x.MessageDeleted == false);
        }
        public List<Message> GetListTrash(string mail)
        {
            return _messageDal.List(x => x.MessageDeleted == true &&
                                         (x.ReceiverMail == mail || x.SenderMail == mail));
        }

        public void MessageAdd(Message message)
        {
            _messageDal.Insert(message);
        }

        public void MessageDelete(Message message)
        {
            _messageDal.Delete(message);
        }

        public void MessageUpdate(Message message)
        {
            _messageDal.Update(message);           // NotImplementedException yerine
        }
        public int GetInboxUnreadCount(string mail)
        {
            return _messageDal.List(x => x.ReceiverMail == mail && x.MessageStatus == false && x.MessageDeleted == false).Count;
        }

    }
}
