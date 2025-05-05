using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Backend.Domain.Messages.User;

namespace FamilyBudgetTracker.Backend.Data.Validation;

public static class AdditionalPersonalValidation
{
    /// <summary>
    /// Validates the given user.
    /// </summary>
    /// <param name="user">The user to validate.</param>
    /// <returns>Returns the validated <see cref="User"/> object if successful.</returns>
    /// <exception cref="UserNotFoundException">Thrown when the user is null.</exception>
    public static User ValidateUser(this User? user)
    {
        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        return user;
    }


    /// <summary>
    /// Validates the given category.
    /// </summary>
    /// <param name="category">The category to validate.</param>
    /// <param name="userId">The ID of the user that the category should belong to.</param>
    /// <returns>Returns the validated <see cref="Category"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the category is null. </exception>
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
    /// Validates the given personal transaction.
    /// </summary>
    /// <param name="transaction">The personal transaction to validate.</param>
    /// <param name="userId">The ID of the user that the transaction should belong to.</param>
    /// <returns>Returns the validated <see cref="PersonalTransaction"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the transaction is null. </exception>
    /// <exception cref="InvalidOperationException">Thrown when the transaction does not belong to the specified user.</exception>
    public static PersonalTransaction ValidatePersonalTransaction(this PersonalTransaction? transaction, string userId)
    {
        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionValidationMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new OperationNotAllowedException(PersonalTransactionValidationMessages.TransactionIsNotFromTheUser);
        }

        return transaction;
    }

    public static PersonalTransaction ValidatePersonalTransactionCategory(this PersonalTransaction transaction, int categoryId)
    {
        if (transaction.Category.Id != categoryId)
        {
            throw new OperationNotAllowedException(PersonalTransactionValidationMessages.CategoryIsNotFromTransaction);
        }

        return transaction;
    }

    /// <summary>
    /// Validates the given recurring transaction.
    /// </summary>
    /// <param name="transaction">The recurring transaction to validate.</param>
    /// <param name="userId">The ID of the user to validate ownership of the transaction.</param>
    /// <returns>Returns the validated <see cref="RecurringTransaction"/> object if successful.</returns>
    /// <exception cref="ResourceNotFoundException">Thrown when the recurring transaction is null.</exception>
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

    public static RecurringTransaction ValidateRecurringTransactionCategory(this RecurringTransaction transaction, int categoryId)
    {
        if (transaction.Category.Id != categoryId)
        {
            throw new OperationNotAllowedException(RecurringTransactionValidationMessages.CategoryIsNotForTransaction);
        }

        return transaction;
    }
}