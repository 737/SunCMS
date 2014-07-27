using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Entity;

namespace Sun.ViewModel.Pagelet
{
    public class ModelFriendLinkUpdate
    {
        public Entity.Pagelet.EntityFriendLink FriendLink { get; set; }

        public List<Entity.Pagelet.EntityFriendLinkGroup> FriendLinkGroup { get; set; }
    }
}
