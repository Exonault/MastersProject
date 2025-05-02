using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Exceptions;

namespace FamilyBudgetTracker.Backend.Util;

public static class AdditionalFamilialValidation
{
    /// <summary>
    /// Validates if the user is associated with the specified family.
    /// </summary>
    /// <param name="user">The user whose family association is being validated.</param>
    /// <param name="familyId">The identifier of the family.</param>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the user does not belong to any family.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the user is not associated with the specified family.
    /// </exception>
    public static void ValidateUserFamily(this User user, Guid familyId)
    {
        if (user.Family is null)
        {
            throw new ResourceNotFoundException(UserMessages.ValidationMessages.NoFamilyForUser);
        }

        if (user.Family.Id != familyId)
        {
            throw new OperationNotAllowedException(UserMessages.ValidationMessages.UserIsNotFromFamily);
        }
    }

    /// <summary>
    /// Validates the given family, ensuring it exists
    /// </summary>
    /// <param name="family">The family instance to validate.</param>
    /// <returns>The validated family instance.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the provided family instance is null.
    /// </exception>
    public static Family ValidateFamily(this Family? family)
    {
        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyMessages.FamilyNotFound);
        }

        return family;
    }

    public static FamilyCategory ValidateFamilyCategory(this FamilyCategory? familyCategory, string familyId)
    {
        if (familyCategory is null)
        {
            throw new ResourceNotFoundException(FamilyCategoryMessages.FamilyCategoryNotFound);
        }

        if (familyCategory.Family.Id != Guid.Parse(familyId))
        {
            throw new OperationNotAllowedException(FamilyCategoryMessages.FamilyCategoryIsNotFromFamily);
        }

        return familyCategory;
    }

    public static FamilyTransaction ValidateFamilyTransaction(this FamilyTransaction? familyTransaction, int familyId)
    {
        return familyTransaction!;
    }
}