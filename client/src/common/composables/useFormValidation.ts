import { reactive } from 'vue';

export default function useFormValidation() {
  const errors: { [key: string]: string } = reactive({});

  const isFormValid = (data: { [key: string]: any }): boolean => {
    const hasEmptyData = Object.values(data).some((value) => value === '');
    const hasErrors = Object.values(errors).some((error) => error !== '');
    return !hasEmptyData && !hasErrors;
  };

  const validateEmptyField = (value: string, fieldName: string) => {
    errors[fieldName] =
      value === ''
        ? `${
            fieldName.charAt(0).toUpperCase() + fieldName.slice(1)
          } can not be empty.`
        : '';
  };

  // regex validates that password contains
  // at least 8 characters
  // at least one non alphanumeric character
  // at least one upper case character
  const passwordPattern = /^(?=.*[A-Z])(?=.*\W).{8,}$/;

  const validatePassword = (value: string) => {
    errors['password'] =
      !passwordPattern.test(value) || value === ''
        ? 'Password must contain at least 8 characters including one non alphanumeric and one upper case character.'
        : '';
  };

  const validatePasswordConfirmation = (
    password: string,
    confirmation: string
  ) => {
    const isPasswordValid = passwordPattern.test(password);
    if (isPasswordValid && confirmation === '') {
      errors['passwordConfirmation'] = 'Confirm your password.';
    } else if (isPasswordValid && password !== confirmation) {
      errors['passwordConfirmation'] = 'Password and confirmation must match.';
    } else {
      errors['passwordConfirmation'] = '';
    }
  };

  const validatePositiveValue = (value: number, fieldName: string) => {
    errors[fieldName] =
      value <= 0
        ? `${
            fieldName.charAt(0).toUpperCase() + fieldName.slice(1)
          } must be greater than 0.`
        : '';
  };

  const clearFormErrors = () => {
    Object.keys(errors).forEach((key) => {
      delete errors[key];
    });
  };

  return {
    errors,
    isFormValid,
    validateEmptyField,
    validatePassword,
    validatePasswordConfirmation,
    validatePositiveValue,
    clearFormErrors,
  };
}
