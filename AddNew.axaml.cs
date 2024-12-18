using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting.Unicode;
using MsBox.Avalonia;
using shubenok1212.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace shubenok1212;

public partial class AddNew : Window
{
    public bool addDopCheck = false;
    public bool actCheck;
    public string path;
    public string filename;
    public string destpath;
    List<string> mans = Helper.context.Manufs.Select(x => x.Name).ToList();
    public Bitmap bitmap;
    public bool isnew;
    public List<Prod> currdops = new List<Prod>();
    public Prod prod = new Prod();
    public AddNew()
    {
        InitializeComponent(); isnew = true;
        List<Prod> newdops = Helper.context.Prods.ToList();
        newdopList.ItemsSource = newdops;
        manCb.ItemsSource = mans;
        prod.IdProd = Helper.context.Prods.OrderBy(x => x.IdProd).Last().IdProd + 1;
        Helper.context.Prods.Add(prod);
        Helper.context.SaveChanges();



    }
    public AddNew(Prod pr)
    {
        InitializeComponent();
        gridAdd.DataContext = pr;
        isnew = false;
        prod.IdProd = pr.IdProd;
        prod.Name = pr.Name;
        prod.IdDops = pr.IdDops;
        prod.IdManufNavigation = pr.IdManufNavigation;
        prod.IdManuf = pr.IdManuf;
        prod.Cost = pr.Cost;
        prod.IdActivNavigation = pr.IdActivNavigation;
        prod.IdActiv = pr.IdActiv;
        nameTx.Text = pr.Name;
        costTx.Text = pr.Cost.ToString();
        prod.Image = pr.Image;
        idTx.Text = pr.IdProd.ToString();
        manCb.ItemsSource = mans;
        manCb.SelectedItem = Helper.context.Manufs.Where(x => x.IdManuf == pr.IdManuf).First().Name;
        img.Source = pr.bitm;
        List<Prod> newdops = Helper.context.Prods.ToList();
        newdopList.ItemsSource = newdops;
        List<Prod> currdops = new List<Prod>(); ;
        if (prod.IdActiv == 1)
        {
            actCh.IsChecked = true;
        }
        else
        {
            actCh.IsChecked = false;
        }

        currdops = prod.IdDops.ToList();
        currdopList.ItemsSource = currdops;
        newdopList.ItemsSource = Helper.context.Prods.ToList();

    }
    private void nazad_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        this.Close();
    }

    private async void img_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog op = new OpenFileDialog();

        var res = await op.ShowAsync(this);
        if (res == null) return;
        path = string.Join("", res);

        if (res != null)
        {
            bitmap = new Bitmap(path);
        }
        img.Source = bitmap;

        destpath = AppDomain.CurrentDomain.BaseDirectory + "Assets";
        filename = Path.GetFileName(path);
        
       
    }

    private void Ok_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        bool check = true;
        if (isnew)
        {
            prod.IdProd = Helper.context.Prods.OrderBy(x => x.IdProd).Last().IdProd + 1;
        }

        if (nameTx.Text.Length > 0)
        {
            prod.Name = nameTx.Text;

        }
        else
        {
            check = false;
            var ms = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не заполнено поле наименования товара");
            ms.ShowAsync();
        }
        string costT = costTx.Text;
        float costCheck;
        if (float.TryParse(costT, out costCheck))
        {
            if (costCheck > 0)
                prod.Cost = costCheck;
            else
            {
                check = false;
                var ms = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неверные данные в поле стоимости");
                ms.ShowAsync();
            }
        }
        else
        {
            check = false;
            var ms = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неверные данные в поле стоимости");
            ms.ShowAsync();
        }
        
            prod.Descr = descrTx.Text;

       
        if (manCb.SelectedIndex != -1)
        {

            Manuf manuf = new Manuf();
            var man = manCb.SelectedItem as string;
            manuf = Helper.context.Manufs.Where(x => x.Name == man).FirstOrDefault();
            prod.IdManufNavigation = manuf;
            prod.IdManuf = manuf.IdManuf;

        }
        else
        {
            check = false;
            var ms = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Не выбран производитель");
            ms.ShowAsync();
        }
        if (actCheck == true)
        {
            prod.IdActivNavigation = Helper.context.Activities.Where(x => x.Name == "Активен").First();
            prod.IdActiv = Helper.context.Activities.Where(x => x.Name == "Активен").First().IdActiv;
        }
        else
        {
            prod.IdActivNavigation = Helper.context.Activities.Where(x => x.Name == "Неактивен").First();
            prod.IdActiv = Helper.context.Activities.Where(x => x.Name == "Неактивен").First().IdActiv;
        }
        if (filename != null)
        {
            prod.Image = filename;
            if (!File.Exists(destpath + filename))
            {
                File.Move(path, destpath + "\\" + filename);
            }
        }
        else
        {
        }
        if (check && isnew)
        {
            prod.IdDops = currdops;
            Helper.context.Prods.Add(prod);
            Helper.context.SaveChanges();
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        else if (check && !isnew)
        {
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().Name = prod.Name;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().Image = prod.Image;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().IdManufNavigation = prod.IdManufNavigation;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().IdManuf = prod.IdManuf;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().IdActivNavigation = prod.IdActivNavigation;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().IdActiv = prod.IdActiv;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().Cost = prod.Cost;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().Descr = prod.Descr;
            Helper.context.Prods.Where(x => x.IdProd == prod.IdProd).First().IdDops = prod.IdDops;
            Helper.context.SaveChanges();
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();

        }
    }


    private void newDopListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var item = newdopList.SelectedItem as Prod;
        if (item != prod)
        {
            prod.IdDops.Add(item);
            currdops = prod.IdDops.ToList();
            currdopList.ItemsSource = currdops;

            /*

              public Bitmap bitm => Image != null && Image != "" ? new Bitmap($@"Assets\\{Image}") : null;
                public string color => IdAct == 2 ?
                    "Gray" : "White";
                public string activ => IdActNavigation.NameAct == "Нет" ?
                    "неактивен" : null;

             */

        }



    }

    private void CurrPrListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var item = currdopList.SelectedItem as Prod;
        if (item != null)
        {
            prod.IdDops.Remove(item);
            currdops = prod.IdDops.ToList();
            currdopList.ItemsSource = currdops;
            addDopCheck = true;
        }
    }
    private void FillCurrDops()
    {
       
    }

    private void CheckBox_IsCheckedChanged_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        if (actCh.IsChecked == true)
        {
            actCheck = true;

        }
        else
        {
            actCheck = false;
        }

    }

    private void delButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }

    private void addButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}