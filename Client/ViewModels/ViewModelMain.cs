using Client.Commands;
using Client.Models;
using System.Windows.Input;

namespace Client.ViewModels;

public class ViewModelMain : ViewModelBase
{
    public ViewModelMain()
    {
        PackageSent = new();
        Sum = "Нет соединения с сервером";

    }

    public ViewModelPackage PackageSent { get; set; }

    ICommand generate;

    public ICommand Generate
    {
        get
        {
            return generate ??= new RelayCommand(obj =>
            {
                Random random = new();

                foreach (var item in PackageSent.Data)
                {
                    item.Index1 = random.Next(16);
                    item.Index2 = random.Next(16);
                }
                PackageSent.UpdateFullPackageAsString();
            });
        }
    }


    public void UpdateSizeMainPacket()
    {
        int currentCount = PackageSent.Data.Count;

        if (currentCount < PackageSent.MainPacketBytes)
        {
            var itemsToAdd = new List<DualByte>(PackageSent.MainPacketBytes - currentCount);
            for (int i = 0; i < PackageSent.MainPacketBytes - currentCount; i++)
            {
                itemsToAdd.Add(new(0, 0));
            }

            foreach (var item in itemsToAdd)
            {
                PackageSent.Data.Add(item);
            }
        }
        else if (currentCount > PackageSent.MainPacketBytes)
        {

            for (int i = currentCount - 1; i >= PackageSent.MainPacketBytes; i--)
            {
                PackageSent.Data.RemoveAt(i);
            }
        }
        PackageSent.UpdateFullPackageAsString();
    }

    string sum;


    public string Sum
    {
        get => sum;
        set
        {
            SetProperty(ref sum, value);
        }
    }
}