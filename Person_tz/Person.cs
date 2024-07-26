using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Person
{
    private string identity;
    private string lastName;
    private string firstName;
    private string surname;
    private string birthDate;
    private string email;
    private string phoneNumber;


    public string Identity
    {
        get { return identity; }
        set
        {
            if (IsValidIdentity(value))
            {
                identity = value;
            }
            else
            {
                throw new ArgumentException("Id должен быть строкой сотоящей из 20 цифр");
            }
        }
    }
    public string LastName
    {
        get { return lastName; }
        set
        {
            if (IsValidName(value))
            {
                lastName = value;
            }
            else
            {
                throw new ArgumentException("Фамилия должна быть строкой до 50 символов, состоящей из букв кириллицы");
            }
        }
    }
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (IsValidName(value))
            {
                firstName = value;
            }
            else
            {
                throw new ArgumentException("Имя должно быть строкой до 50 символов, состоящей из букв кириллицы");
            }
        }
    }
    public string Surname
    {
        get { return surname; }
        set
        {
            if (!IsValidName(value))
            {
                surname = value;
            }
            else
            {
                throw new ArgumentException("Отчество должно быть строкой до 50 символов, состоящей из букв кириллицы");
            }
        }
    }
    public string BirthDate 
    { 
        get { return birthDate; }
        set 
        {
            if (IsValidDate(value))
            {
                birthDate = value;
            }
            else {
                throw new ArgumentException("Неверный формат даты рождения. Используйте формат «дд.ММ.гггг»");
                 }
        }
    }
    public string Email
    {
        get { return email; }
        set
        {
            if (IsValidEmail(value))
            {
                email = value;
            }
            else
            {
                throw new ArgumentException("Неверный формат адреса.");
            }
        }
    }
    public string PhoneNumber
    {
        get { return phoneNumber; }
        set
        {
            if (IsValidPhoneNumber(value))
            {
                phoneNumber = value;
            }
            else
            {
                throw new ArgumentException("Неверный формат номера телефона.");
            }
        }
    }

    public Person(string identity, string lastName, string firstName, string surname, string birthDate, string email, string phoneNumber)
    {
        Identity = identity;
        LastName = lastName;
        FirstName = firstName;
        Surname = surname;
        BirthDate = birthDate;
        Email = email;
        PhoneNumber = phoneNumber;
    }

private bool IsValidIdentity(string id)
{
    if (string.IsNullOrEmpty(id) || id.Length > 20)
            return false;

    foreach (char c in id)
    {
        if (!char.IsDigit(c))
            return false;
    }
    return true;
}

    private bool IsValidName(string name)
    {
        if(string.IsNullOrEmpty(name) || name.Length > 50)
                return false;
        if(!Regex.IsMatch(name, @"\P{IsCyrillic}"))
        {
            return true;
        }
        //foreach(char c in name)
        //{
        //    if(!char.IsLetter(c) || !char.IsLetter(c))
        //        return false;
        //}
        // return true;
        else return false;
    }

    private bool IsValidDate(string date)
    {
        DateTime parsedDate;
        return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate);
    }
    private bool IsValidEmail(string email) 
    {
        return email.Contains("@") && email.Length <= 100;
    }
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Length <= 100;
    }
}
    
