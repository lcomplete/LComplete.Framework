using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LComplete.Framework.Data;
using NUnit.Framework;

namespace LComplete.Framework.UnitTests.Data
{
    class TestDbModel
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Title { get; set; }
    }

    [TestFixture]
    public class OrderFieldStoreTests
    {
        private OrderFieldStore<TestDbModel> GetIdFieldStore()
        {
            OrderFieldStore<TestDbModel> fieldStore = new OrderFieldStore<TestDbModel>();
            fieldStore.Add(t => t.Id);
            return fieldStore;
        }

        private OrderFieldStore<TestDbModel> GetMultiAscendingFieldStore()
        {
            var fieldStore = GetIdFieldStore();
            fieldStore.Add(t => t.CreateDate, OrderType.Ascending);
            fieldStore.Add(t => t.Title, OrderType.Ascending);
            return fieldStore;
        }

        [Test]
        public void Add_OneField_PriorityBeOne()
        {
            var fieldStore = GetIdFieldStore();
            OrderField<TestDbModel> field = fieldStore.GetOrderField(t => t.Id);
            Assert.AreEqual(1, field.Priority);
        }

        [Test]
        public void GetOrderField_UseOrderKey_GetIdField()
        {
            var fieldStore = GetIdFieldStore();
            OrderField<TestDbModel> field = fieldStore.GetOrderField("Id");
            Assert.AreEqual("Id", field.OrderKey);
        }

        [Test]
        public void MakeOrderFlags_NotKeepRawOrder_RightFlags()
        {
            var fieldStore = GetMultiAscendingFieldStore();
            fieldStore.SetOrderField(t => t.Title, OrderType.Descending);

            string orderFlags = fieldStore.MakeOrderFlags(t => t.CreateDate, OrderType.Descending);
            Assert.AreEqual("-2.-3", orderFlags);
        }

        [Test]
        public void MakeOrderFlags_KeepRawOrder_RightFlags()
        {
            var fieldStore = GetMultiAscendingFieldStore();
            fieldStore.SetOrderField(t => t.Title, OrderType.Descending);

            string orderFlags = fieldStore.MakeOrderFlags(t => t.CreateDate, OrderType.Descending,
                isOtherUseRawOrder: true);
            Assert.AreEqual("-2.3", orderFlags);
        }

        [Test]
        public void ChangeOrderFlags_ChangeOneOrderType_Right()
        {
            var fieldStore = GetMultiAscendingFieldStore();
            fieldStore.ChangeOrderFlags("-1");
            var idField = fieldStore.GetOrderField(t => t.Id);
            Assert.AreEqual(OrderType.Descending, idField.OrderType);
        }

        [Test]
        public void ChangeOrderFlags_ChangeMultiOrderType_Right()
        {
            var fieldStore = GetMultiAscendingFieldStore();
            fieldStore.ChangeOrderFlags("-1.-2.-3");

            var idField = fieldStore.GetOrderField(t => t.Id);
            Assert.AreEqual(OrderType.Descending, idField.OrderType);

            var dateField = fieldStore.GetOrderField(t => t.CreateDate);
            Assert.AreEqual(OrderType.Descending, dateField.OrderType);

            var titleField = fieldStore.GetOrderField(t => t.Title);
            Assert.AreEqual(OrderType.Descending, titleField.OrderType);
        }
    }
}
