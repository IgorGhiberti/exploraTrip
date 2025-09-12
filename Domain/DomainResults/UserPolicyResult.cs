using Domain.Entities;

namespace Domain.DomainResults;

public static class UserPolicyResult
{
    public static ResultData<User?> IsUserNullOrDisable(User? user)
    {
        if (user == null)
            return ResultData<User?>.Error("User does not exist.");

        if (!user.Active)
            return ResultData<User?>.Error("User is disabled.");

        return ResultData<User?>.Success(user);
    }
}