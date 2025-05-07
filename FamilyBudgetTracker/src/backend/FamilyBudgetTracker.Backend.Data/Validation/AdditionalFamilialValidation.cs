using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Backend.Domain.Messages.User;

namespace FamilyBudgetTracker.Backend.Data.Validation;

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
            throw new ResourceNotFoundException(UserValidationMessages.NoFamilyForUser);
        }

        if (user.Family.Id != familyId)
        {
            throw new OperationNotAllowedException(UserValidationMessages.UserIsNotFromFamily);
        }
    }

    /// <summary>
    /// Validates if the user is associated with a family.
    /// </summary>
    /// <param name="user">The user whose family association is being validated.</param>
    /// <exception cref="OperationNotAllowedException">
    /// Thrown when the user does belong to any family.
    /// </exception>
    public static User  ValidateUserHasFamily(this User user)
    {
        if (user.Family is not null)
        {
            throw new OperationNotAllowedException(UserValidationMessages.UserHasFamily);
        }

        return user;
    }

    /// <summary>
    /// Validates the given family.
    /// </summary>
    /// <param name="family">The family to be validated.</param>
    /// <returns>The validated family instance.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the provided family instance is null.
    /// </exception>
    public static Family ValidateFamily(this Family? family)
    {
        if (family is null)
        {
            throw new ResourceNotFoundException(FamilyValidationMessages.FamilyNotFound);
        }

        return family;
    }

    /// <summary>
    /// Validates the given family category.
    /// </summary>
    /// <param name="familyCategory">The family category to be validated.</param>
    /// <param name="familyId">The identifier of the associated family.</param>
    /// <returns>The validated family category.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified family category is null or does not exist.
    /// </exception>
    /// <exception cref="OperationNotAllowedException">
    /// Thrown when the family category is not associated with the specified family.
    /// </exception>
    public static FamilyCategory ValidateFamilyCategory(this FamilyCategory? familyCategory, Guid familyId)
    {
        if (familyCategory is null)
        {
            throw new ResourceNotFoundException(FamilyCategoryValidationMessages.FamilyCategoryNotFound);
        }

        if (familyCategory.Family.Id != familyId)
        {
            throw new OperationNotAllowedException(FamilyCategoryValidationMessages.FamilyCategoryIsNotFromFamily);
        }

        return familyCategory;
    }

    /// <summary>
    /// Validates the given family transaction.
    /// </summary>
    /// <param name="familyTransaction">The family transaction to br validated.</param>
    /// <param name="familyId">The ID of the family to validate association with.</param>
    /// <param name="userId">The ID of the user to validate association with.</param>
    /// <returns>The validated family transaction.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the specified family transaction is null.
    /// </exception>
    /// <exception cref="OperationNotAllowedException">
    /// Thrown when the family transaction is not associated with the given family.
    /// </exception>
    public static FamilyTransaction ValidateFamilyTransaction(this FamilyTransaction? familyTransaction, Guid familyId, string userId)
    {
        if (familyTransaction is null)
        {
            throw new ResourceNotFoundException(FamilyTransactionValidationMessages.NoTransactionFound);
        }

        if (familyTransaction.Family.Id != familyId)
        {
            throw new OperationNotAllowedException(FamilyTransactionValidationMessages.TransactionIsNotFromTheFamily);
        }

        if (familyTransaction.User.Id != userId)
        {
            throw new OperationNotAllowedException(FamilyTransactionValidationMessages.TransactionIsNotFromTheUser);
        }

        return familyTransaction;
    }


    public static FamilyTransaction ValidateFamilyTransactionCategory(this FamilyTransaction familyTransaction, int familyCategoryId)
    {
        if (familyTransaction.Category.Id != familyCategoryId)
        {
            throw new OperationNotAllowedException(FamilyTransactionValidationMessages.CategoryNotFromTransaction);
        }

        return familyTransaction;
    }
}