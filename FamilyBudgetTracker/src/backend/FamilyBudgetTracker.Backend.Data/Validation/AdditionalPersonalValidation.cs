using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Backend.Domain.Messages.User;

namespace FamilyBudgetTracker.Backend.Data.Validation;

public static class AdditionalPersonalValidation
{
    /// <summary>
    /// Validates the given user, ensuring it exists
    /// </summary>
    /// <param name="user">The user to validate. Throws an exception if null.</param>
    /// <returns>Returns the validated <see cref="User"/> object if successful.</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user is not found.</exception>
    public static User ValidateUser(this User? user)
    {
        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        return user;
    }


    /// <summary>
    /// Validates the given category, ensuring it exists and belongs to the specified user.
    /// </summary>
    /// <param name="category">The category to validate. Throws an exception if null.</param>
    /// <param name="userId">The ID of the user that the category should belong to.</param>
    /// <returns>Returns the validated <see cref="Category"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the category is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the category does not belong to the specified user.</exception>
    public static Category ValidateCategory(this Category? category, string userId)
    {
        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryValidationMessages.NoCategoryFound);
        }

        if (category.User.Id != userId)
        {
            throw new OperationNotAllowedException(CategoryValidationMessages.CategoryIsNotFromTheUser);
        }

        return category;
    }


    /// <summary>
    /// Validates the given personal transaction, ensuring it exists and belongs to the specified user.
    /// </summary>
    /// <param name="transaction">The personal transaction to validate. Throws an exception if null.</param>
    /// <param name="userId">The ID of the user that the transaction should belong to.</param>
    /// <returns>Returns the validated <see cref="PersonalTransaction"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the transaction is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the transaction does not belong to the specified user.</exception>
    public static PersonalTransaction ValidatePersonalTransaction(this PersonalTransaction? transaction, string userId)
    {
        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionValdationMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new OperationNotAllowedException(PersonalTransactionValdationMessages.TransactionIsNotFromTheUser);
        }

        return transaction;
    }


    /// <summary>
    /// Validates the given recurring transaction, ensuring it exists and belongs to the specified user.
    /// </summary>
    /// <param name="transaction">The recurring transaction to validate. Throws an exception if null.</param>
    /// <param name="userId">The ID of the user to validate ownership of the transaction.</param>
    /// <returns>Returns the validated <see cref="RecurringTransaction"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the recurring transaction is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the recurring transaction does not belong to the specified user.</exception>
    public static RecurringTransaction ValidateRecurringTransaction(this RecurringTransaction? transaction, string userId)
    {
        if (transaction is null)
        {
            throw new ResourceNotFoundException(RecurringTransactionValidationMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new OperationNotAllowedException(RecurringTransactionValidationMessages.TransactionIsNotFromTheUser);
        }

        return transaction;
    }
}