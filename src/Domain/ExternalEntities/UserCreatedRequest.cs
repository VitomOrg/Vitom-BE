namespace Domain.ExternalEntities;

public class EmailAddress
{
    public string email_address { get; set; } = string.Empty;
    public string id { get; set; } = string.Empty;
    public List<string> linked_to { get; set; } = new List<string>();
    public string object_type { get; set; } = "email_address";
    public Verification verification { get; set; } = new Verification();
}

public class Verification
{
    public string status { get; set; } = "unverified";
    public string strategy { get; set; } = "none";
}

public class EventAttributes
{
    public HttpRequest http_request { get; set; } = new HttpRequest();
}

public class HttpRequest
{
    public string client_ip { get; set; } = "0.0.0.0";
    public string user_agent { get; set; } = string.Empty;
}

public class ClerkUser
{
    public string birthday { get; set; } = string.Empty;
    public long created_at { get; set; } = 0L;
    public List<EmailAddress> email_addresses { get; set; } = new List<EmailAddress>();
    public List<string> external_accounts { get; set; } = new List<string>();
    public string external_id { get; set; } = string.Empty;
    public string first_name { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    public string id { get; set; } = string.Empty;
    public string image_url { get; set; } = string.Empty;
    public string last_name { get; set; } = string.Empty;
    public long last_sign_in_at { get; set; } = 0L;
    public string object_type { get; set; } = "user";
    public bool password_enabled { get; set; } = false;
    public List<string> phone_numbers { get; set; } = new List<string>();
    public string primary_email_address_id { get; set; } = string.Empty;
    public string primary_phone_number_id { get; set; } = string.Empty;
    public string primary_web3_wallet_id { get; set; } = string.Empty;
    public Dictionary<string, string> private_metadata { get; set; } = new Dictionary<string, string>();
    public string profile_image_url { get; set; } = string.Empty;
    public Dictionary<string, string> public_metadata { get; set; } = new Dictionary<string, string>();
    public bool two_factor_enabled { get; set; } = false;
    public Dictionary<string, string> unsafe_metadata { get; set; } = new Dictionary<string, string>();
    public long updated_at { get; set; } = 0L;
    public string username { get; set; } = string.Empty;
    public List<string> web3_wallets { get; set; } = new List<string>();
}

public class UserCreatedClerkEvent
{
    public ClerkUser data { get; set; } = new ClerkUser();
    public EventAttributes event_attributes { get; set; } = new EventAttributes();
    public string object_type { get; set; } = "event";
    public long timestamp { get; set; } = 0L;
    public string type { get; set; } = string.Empty;
}