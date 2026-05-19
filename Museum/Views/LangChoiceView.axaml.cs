using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Museum.ViewModels;
using ReactiveUI;

namespace Museum.Views;

public partial class LangChoiceView : ReactiveUserControl<LangChoiceViewModel>
{
    public LangChoiceView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.FrenchButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.DutchButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.GermanButton).DisposeWith(disposable);
            this.BindCommand(ViewModel, vm => vm.SelectLang, v => v.EnglishButton).DisposeWith(disposable);
        });
    }
}