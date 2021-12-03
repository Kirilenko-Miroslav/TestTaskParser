# TestTaskParser
**Задание:**<br /><br />
Написать парсер страницы - https://www.lesegais.ru/open-area/deal<br />
Утилита должна обойти все записи и сохранить информацию в формате JSON в БД.<br />
После обхода записей, должна проверять наличие новых и добавлять их в базу, делая запрос 1 раз в 10 минут.<br />
Оформить в виде консольного приложения .Net Framework (не Core). Скорость выполнения важнее красоты решения. Нужно простое, но рабочее решение. Необходимо сообщить трудозатраты в часах.<br /><br />

**О трудозатратах в часах:** +-10-11ч<br /><br />
**Первый день(+-5ч):** Первые пол-часа час я, посмотрев на то какие запросы я отправляю при просмотре БД, пытался обратиться к БД через запросы но у меня не вышло и я попробовал другим способом. Следующие 4 часа я создавал и тестировал приложение с веб-драйвером (Selenium) которое запускает браузер и вручную щелкает страницы. Т.к. до этого с парсингом веб приложений я не сталкивался я подумал что это может быть рабочий вариант, но, после тестирования, я посчитал что на моём конфиге текущая БД считывается за +-7.5 часов рабочего времени и решил поискать более быстрое решение.<br /><br />
**Второй день(+-5-6ч):** Я снова решил попробовать создать запрос к graphql и получилось, и дальше я создавал и тестировал приложение. Но я столкнулся с неопределенностью того как добавляются данные в БД, потому что: при запросе к graphql, даже с учетом фильтра по дате (последнее - первым), оно отправляет данные в каком-то не очень понятном порядке (на сайте вживую - одно, при парсинге - другое) и еще час-2 разбирался и читал различные документации, и решил оставить пока так, ибо лучшего решения я не нашел. Если все же будет потребность искать не добавленные данные - то я думаю это можно делать с помощью прохода и с помощью поля "Номер декларации" и с помощью метода graphql "NIN". А также есть вопросы по полям типа "дата - 28.04.2116" и т.д.
