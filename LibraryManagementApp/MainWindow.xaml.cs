using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseHandler dbHandler;
        BindingList<Book> listBooks;

        public MainWindow()
        {
            InitializeComponent();
            listBooks = new BindingList<Book>();
            dbHandler = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetAllBooksFromDB();
        }

        private void GetAllBooksFromDB()
        {
            dbHandler = DatabaseHandler.getInstance();
            dbHandler.connection.Open();

            SqlCommand cmd = new SqlCommand("SP_GetAllBooks", dbHandler.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            PopulateListViewBooks(dt);
            dbHandler.connection.Close();
        }

        private void PopulateListViewBooks(DataTable dt)
        {
            if (listBooks.Count != 0)
                listBooks.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Book book = new Book();
                book.ID = int.Parse(dt.Rows[i][0].ToString());

                book.Name = dt.Rows[i][1].ToString();

                book.Category = dt.Rows[i][2].ToString();

                book.Author = dt.Rows[i][3].ToString();

                book.Publisher = dt.Rows[i][4].ToString();

                if (dt.Rows[i][5] == System.DBNull.Value)
                { book.YearPublished = -1; }
                else
                { book.YearPublished = int.Parse(dt.Rows[i][5].ToString()); }

                if (dt.Rows[i][6] == System.DBNull.Value)
                { book.Price = -1; }
                else
                { book.Price = int.Parse(dt.Rows[i][6].ToString()); }

                book.ISBN = dt.Rows[i][7].ToString();

                listBooks.Add(book);
            }

            listViewBooks.ItemsSource = listBooks;
        }

        /* Menu Items Actions*/
        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(this,
                "Nguyễn Khánh Hoàng - 1712457",
                "Thông tin tác giả");
        }

        private void ListViewBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow abWindow = new AddBookWindow();
            abWindow.saveNewBook += AddNewBookToDB;
            abWindow.ShowDialog();
        }

        private void AddNewBookToDB(Book newBook)
        {
            dbHandler.connection.Open();

            SqlCommand cmd = new SqlCommand("SP_AddNewBook", dbHandler.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", SqlDbType.Int).Value = newBook.ID;
            cmd.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = newBook.Name;
            cmd.Parameters.AddWithValue("@Category", SqlDbType.NVarChar).Value = newBook.Category;
            cmd.Parameters.AddWithValue("@Author", SqlDbType.NVarChar).Value = newBook.Author;
            cmd.Parameters.AddWithValue("@Publisher", SqlDbType.NVarChar).Value = newBook.Publisher;
            cmd.Parameters.AddWithValue("@YearPublished", SqlDbType.Int).Value = newBook.YearPublished;
            cmd.Parameters.AddWithValue("@Price", SqlDbType.Int).Value = newBook.Price;
            cmd.Parameters.AddWithValue("@ISBN", SqlDbType.NVarChar).Value = newBook.ISBN;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error");
                dbHandler.connection.Close();
                throw ex;
            }

            dbHandler.connection.Close();
            GetAllBooksFromDB();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int selected = listViewBooks.SelectedIndex;
            if (selected == -1)
            {
                MessageBox.Show("Vui lòng chọn cuốn sách cần xóa", "Xóa Sách...");
                return;
            }
            else
            {
                if (MessageBox.Show(this, "Bạn có thực sự muốn xóa?", "Xóa sách...", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }
                else
                {
                    DeleteBookFromDB(listBooks[selected]);
                    GetAllBooksFromDB();
                }
            }
        }

        private void DeleteBookFromDB(Book book)
        {
            dbHandler.connection.Open();

            SqlCommand cmd = new SqlCommand("SP_DeleteBook", dbHandler.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bID", SqlDbType.Int).Value = book.ID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            dbHandler.connection.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có thực sự muốn thoát?", "Thoát chương trình...", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                return;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GetAllBooksFromDB();
        }
    }
}
