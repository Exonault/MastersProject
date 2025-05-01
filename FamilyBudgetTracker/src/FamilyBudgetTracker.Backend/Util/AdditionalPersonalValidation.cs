using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;
using FamilyBudgetTracker.BE.Commons.Exceptions;

namespace FamilyBudgetTracker.Backend.Util;

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
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
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
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }

        if (category.User.Id != userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
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
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.TransactionIsNotFromTheUser);
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
            throw new ResourceNotFoundException(RecurringTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(RecurringTransactionMessages.TransactionIsNotFromTheUser);
        }

        return transaction;
    }
}