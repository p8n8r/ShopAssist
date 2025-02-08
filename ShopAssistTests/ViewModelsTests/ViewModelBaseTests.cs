using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.ViewModels;
using System.ComponentModel;

namespace ShopAssist.ViewModels.Tests
{
    [TestClass]
    public class ViewModelBaseTests
    {
        private class TestViewModel : ViewModelBase
        {
            private string myProperty;
            public string TestProperty
            {
                get { return this.myProperty; }
                set
                {
                    if (this.myProperty != value)
                    {
                        this.myProperty = value;
                        OnPropertyChanged();
                    }
                }
            }
        }

        [TestMethod]
        public void OnPropertyChanged_RaisesPropertyChangedEvent()
        {
            // Arrange
            var viewModel = new TestViewModel();

            bool eventRaised = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(TestViewModel.TestProperty))
                    eventRaised = true;
            };

            // Act
            viewModel.TestProperty = "New Value";

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
