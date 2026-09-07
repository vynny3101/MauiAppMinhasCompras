using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class LiastaProduto : ContentPage
{
	ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();

	public LiastaProduto()
	{
		InitializeComponent();

		Lst_produtos.ItemsSource = Lista;
	}

    protected async override void OnAppearing()
    {
		try
		{
            Lista.Clear();

		List<Produto>  tmp = await App.Db.GetALL();

		tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		}
        catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "Ok");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		try
		{

		string q = e.NewTextValue;

		Lista.Clear();

        List<Produto> tmp = await App.Db. Search(q);

        tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = Lista.Sum(i => i.total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "Ok");
    }

    private async Task MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Remover {p.Descricao}", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                Lista.Remove(p);
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }

    }

    private void Lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}