namespace Gahar_Backend.Constants;

public static class FormFieldTypes
{
    public const string Text = "Text";
    public const string Email = "Email";
    public const string Number = "Number";
    public const string Select = "Select";
    public const string MultiSelect = "MultiSelect";
    public const string Checkbox = "Checkbox";
    public const string Radio = "Radio";
  public const string TextArea = "TextArea";
    public const string DateTime = "DateTime";
    public const string Date = "Date";
    public const string Time = "Time";
    public const string Phone = "Phone";
    public const string Url = "Url";
  public const string File = "File";
    public const string Hidden = "Hidden";

    public static readonly string[] AllFieldTypes = new[]
    {
        Text, Email, Number, Select, MultiSelect, Checkbox, Radio,
TextArea, DateTime, Date, Time, Phone, Url, File, Hidden
    };

    public static bool IsValid(string fieldType)
    {
        return AllFieldTypes.Contains(fieldType);
    }
}
