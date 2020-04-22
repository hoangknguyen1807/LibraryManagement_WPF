using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryManagementApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        bool saved = false;

        public delegate void AddBookDelegate (Book book);
        public event AddBookDelegate saveNewBook;

        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(this, "Bạn đã chắc chắn lưu?", "Lưu...", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Book newBook = new Book();
                try {
                    newBook.ID = int.Parse(txtBoxID.Text);
                    newBook.Name = txtBoxName.Text;
                    newBook.Category = txtBoxCategory.Text;
                    newBook.Author = txtBoxAuthor.Text;
                    newBook.Publisher = txtBoxPublisher.Text;

                    if (txtBoxYearPublished.Text == "")
                    { newBook.YearPublished = -1; }
                    else
                    { newBook.YearPublished = int.Parse(txtBoxYearPublished.Text); }

                    if (txtBoxPrice.Text == "")
                    { newBook.Price = -1; }
                    else
                    { newBook.Price = int.Parse(txtBoxPrice.Text); }

                    // Need To Check ISBN Length <= 20
                    newBook.ISBN = txtBoxISBN.Text;

                    saveNewBook?.Invoke(newBook);
                }
                catch (FormatException fe)
                {
                    MessageBox.Show(this, "Dữ liệu không hợp lệ!\n" +
                        "\"Mã Sách\", \"Năm XB\" và \"Giá trị\" phải là số nguyên", "Lỗi định dạng");
                    return;
                }
                catch (SqlException sqlEx)
                {
                    return;
                }
                saved = true;
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saved == false) // not saved but cancel
            {
                if (MessageBox.Show(this, "Bạn có thực sự muốn hủy?", "Hủy...", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
