using System.Windows;
using System.Windows.Controls;

namespace BoxID_GR1_WPF
{
    static class DataGridExtension
    {
        public static string GetDataField(DependencyObject obj)
        {
            return (string)obj.GetValue(DataFieldProperty);
        }

        public static void SetDataField(DependencyObject obj, string value)
        {
            obj.SetValue(DataFieldProperty, value);
        }

        // Using a DependencyProperty as the backing store for DataField.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataFieldProperty =
            DependencyProperty.RegisterAttached("DataField", typeof(string), 
            typeof(DataGridExtension), new UIPropertyMetadata(""));

        public static object GetCellValue(this DataGrid target, DataGridCellInfo cellInfo)
        {
            string dataField = GetDataField(cellInfo.Column);          
            return GetValueFromProperty(cellInfo.Item, dataField);
        }

        private static object GetValueFromProperty(object target, string propertyName)
        {
            var propertyInfo =target
                .GetType()
                .GetProperty(propertyName);
            return propertyInfo != null ?
                propertyInfo
                .GetGetMethod()
                .Invoke(target, null)
                : null;
        }

    }     
}
