using System.ComponentModel;

namespace Client.Models;

public class DualByte : INotifyPropertyChanged
{
    public DualByte(int index1, int index2)
    {
        Index1 = index1;
        Index2 = index2;
    }

    int index1;
    int index2;

    public int Index1 { get => index1; set { index1 = value; NotifyPropertyChanged(nameof(Index1)); } }

    public int Index2 { get => index2; set { index2 = value; NotifyPropertyChanged(nameof(Index2)); } }

    public event PropertyChangedEventHandler PropertyChanged;

    void NotifyPropertyChanged(string p)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }

    public byte ToByte => (byte)(Index1 * 16 + Index2);
}
