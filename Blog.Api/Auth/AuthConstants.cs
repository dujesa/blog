namespace Blog.Api;

public static class AuthConstants
{
    public const string AdminUserPolicyName = "Admin";
    public const string AdminUserClaimName = "admin";
    
    public const string TrustMemberPolicyName = "Trusted";
    public const string TrustedMemberClaimName = "trusted_member";
}