namespace Rembot.Bus;

public static class Buttons
{
    public const string START = "/start";
    public const string MENU = "📱 Меню";
    public const string ORDERS = "🛠️ Заказы";
    public const string BONUSES = "✨ Бонусы и рефералы";
    public const string CONTACTS = "☎️ Контакты";
    public const string REFERALS = "👨 Список рефералов";
    public const string SHARE_NUMBER = "Поделиться номером";
}

public static class Responses
{
    public const string USER_ALREADY_EXISTS = "Такой пользователь уже зарегистрирован 🙍";
    public const string PHONE_NUMBER_REQUIRED = "Для регистрации необходим ваш номер телефона 🙏";
    public const string CASHBACK = "💰 Кэшбэк";
    public const string DISCOUNT = "➗ Скидка";
    public const string REFERALS_COUNT = "🤝 Количество рефералов";
    public const string REFERAL_LINK = "🚀 Ваша реферальная ссылка";
    public const string CONTACTS_INFO = "☎️ Контакты: \n\n" +
                "Тел: +79998887766 \n" +
                "Почта: rembot@mail.ru \n" + 
                "Адрес: Невский проспект, 100";
    public const string NO_ACTIVE_ORDERS = "Нет активных заказов 😴";
    public const string NO_REFERALS = "У вас пока нет рефералов 😴";
    public const string DEVICE = "📟 Устройство";
    public const string DESCRIPTION = "📜 Описание";
    public const string STATUS = "⚙️ Статус";
}
