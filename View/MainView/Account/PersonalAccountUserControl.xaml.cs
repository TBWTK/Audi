using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using Audi.Model;

namespace Audi.View.MainView.Account
{
    public partial class PersonalAccountUserControl : UserControl
    {
        int idUser;
        List<Genders> genders;
        public PersonalAccountUserControl(int ID)
        {
            idUser = ID;
            InitializeComponent();
            using (var context = new SalesAudiEntities())
            {
                genders = context.Genders.ToList();
            }
            BoxGender.ItemsSource = genders;
            LoadUser();
        }


        private void LoadUser()
        {
            using (var context = new SalesAudiEntities())
            {
                var user = context.Users.SingleOrDefault(x => x.Id == idUser);
                if (user != null)
                {
                    if (user.Name != null)
                    {
                        NameTextBlock.Text = user.Name;
                        NameTextBox.Text = user.Name;
                    }
                    if (user.SureName != null)
                    {
                        SurNameTextBlock.Text = user.SureName;
                        SurNameTextBox.Text = user.SureName;
                    }
                    if (user.LastName != null)
                    {
                        LastNameTextBlock.Text = user.LastName;
                        LastNameTextBox.Text = user.LastName;
                    }
                    if (user.Phone != null)
                    {
                        PhoneTextBlock.Text = user.Phone;
                        PhoneTextBox.Text = user.Phone;
                    }
                    if (user.DateOfBirth != null)
                    {
                        DateTextBlock.Text = $"{user.DateOfBirth.Value.Day}.{user.DateOfBirth.Value.Month}.{user.DateOfBirth.Value.Year}";
                        DateBirth.SelectedDate = user.DateOfBirth.Value.Date;
                    }
                    if (user.Photo != null)
                    {
                        Image image = byteArrayToImage(user.Photo);
                        PhotoUser.Source = image.Source;
                    }
                    if (user.Genders != null)
                        GenderTextBlock.Text = user.Genders.NameGender;
                }
            }
        }

        // Загрузка дефолтных имен
        private void VisibleChangeName(object sender, RoutedEventArgs e)
        {
            DefName.Visibility = Visibility.Hidden;
            ChangeName.Visibility = Visibility.Visible;
        }


        // Конвертация фото в байт и обратно
        public Image byteArrayToImage(byte[] byteArray)
        {
            Image image = new Image();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            return image;
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }


        // Загрузка фото в БД
        private void DowlandPhoto(object sender, RoutedEventArgs e)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            Microsoft.Win32.OpenFileDialog ofdPicture = new Microsoft.Win32.OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.JPG;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                myBitmapImage.UriSource = new Uri(ofdPicture.FileName);
                myBitmapImage.EndInit();
                PhotoUser.Source = myBitmapImage;
                using (var context = new SalesAudiEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                    if (us != null)
                    {
                        us.Photo = getJPGFromImageControl(PhotoUser.Source as BitmapImage);
                        context.SaveChanges();
                        MessageBox.Show("Изменения прошли успешно");
                    }
                }
                LoadUser();
            }
        }


        // Обновление ФИО, гендера пользователя, телефона, даты
        private void UpdateNameUser(object sender, RoutedEventArgs e)
        {
            using (var context = new SalesAudiEntities())
            {
                var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                if (us != null)
                {
                    us.Name = NameTextBox.Text;
                    us.SureName = SurNameTextBox.Text;
                    us.LastName = LastNameTextBox.Text;
                    us.Phone = PhoneTextBox.Text;
                    us.DateOfBirth = DateBirth.SelectedDate.Value.Date;


                    int selectedIndex = BoxGender.SelectedIndex;
                    if (selectedIndex > -1)
                    {
                        Genders selectedItem = (Genders)BoxGender.SelectedItem;
                        us.Gender = selectedItem.Id;
                    }
                    context.Entry(us).State = EntityState.Modified;
                    context.SaveChanges();
                    MessageBox.Show("Изменения прошли успешно");
                }
            }
            DefName.Visibility = Visibility.Visible;
            ChangeName.Visibility = Visibility.Hidden;
            LoadUser();
        }


        // Просмотр пароля
        private void CheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = PassPassBox.Password.Trim();
            PassPassBox.Visibility = Visibility.Hidden;
            PassTextBlock.Visibility = Visibility.Visible;
        }
        private void UnCheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = "";
            PassTextBlock.Visibility = Visibility.Hidden;
            PassPassBox.Visibility = Visibility.Visible;
        }
        private void CheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = PassPassBoxOne.Password.Trim();
            PassPassBoxOne.Visibility = Visibility.Hidden;
            PassTextBlockOne.Visibility = Visibility.Visible;
        }
        private void UnCheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = "";
            PassTextBlockOne.Visibility = Visibility.Hidden;
            PassPassBoxOne.Visibility = Visibility.Visible;
        }
        private void CheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = PassPassBoxTwo.Password.Trim();
            PassPassBoxTwo.Visibility = Visibility.Hidden;
            PassTextBlockTwo.Visibility = Visibility.Visible;
        }
        private void UnCheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = "";
            PassTextBlockTwo.Visibility = Visibility.Hidden;
            PassPassBoxTwo.Visibility = Visibility.Visible;
        }


        // Обновление пароля
        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {

            using (var context = new SalesAudiEntities())
            {
                var us = context.Users.SingleOrDefault(x => x.Id == idUser && x.Password == PassPassBox.Password.Trim());

                if (PassPassBoxOne.Password == PassPassBoxTwo.Password && us != null)
                {
                    us.Password = PassPassBoxOne.Password.Trim();
                    context.Entry(us).State = EntityState.Modified;
                    context.SaveChanges();
                    PassPassBox.Clear();
                    PassPassBoxOne.Clear();
                    PassPassBoxTwo.Clear();
                    MessageBox.Show("Изменения прошли успешно");
                }
                else
                {
                    MessageBox.Show("ВЫ ВВЕЛИ НЕВЕРНЫЙ ПАРОЛЬ!");
                }
            }
            UpdatePassword.Visibility = Visibility.Visible;
            ChangePassword.Visibility = Visibility.Hidden;
        }

        private void VisibleChangePassword(object sender, MouseButtonEventArgs e)
        {
            UpdatePassword.Visibility = Visibility.Hidden;
            ChangePassword.Visibility = Visibility.Visible;
        }
    }
}
