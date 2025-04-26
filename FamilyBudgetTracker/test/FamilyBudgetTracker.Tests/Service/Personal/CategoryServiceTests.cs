using FamilyBudgetTracker.Backend.Services.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Personal;
using FluentValidation;
using NSubstitute;

namespace FamilyBudgetTracker.Tests.Service.Personal;

[TestFixture]
public class CategoryServiceTests
{
    private ICategoryRepository _categoryRepository;
    private IUserRepository _userRepository;
    private IValidator<CreateCategoryRequest> _createValidator;
    private IValidator<UpdateCategoryRequest> _updateValidator;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        // Create substitutes (mocks)
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _createValidator = Substitute.For<IValidator<CreateCategoryRequest>>();
        _updateValidator = Substitute.For<IValidator<UpdateCategoryRequest>>();

        // System Under Test
        _categoryService =
            new CategoryService(_categoryRepository, _userRepository);
    }

    [Test]
    public void CreateCategory_ThrowsUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var request = new CreateCategoryRequest { Name = "Test Category" };
        _userRepository.GetById(userId).Returns((User)null); // Simulate user not found

        // Act & Assert
        var ex = Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await _categoryService.CreateCategory(request, userId));

        Assert.That(ex.Message, Is.EqualTo("User not found."));
        _userRepository.Received(1).GetById(userId); // Ensure `GetById` was called once
    }
}