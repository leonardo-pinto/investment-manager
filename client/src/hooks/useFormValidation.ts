import { reactive } from 'vue';

const errors: { [key: string]: string } = reactive({});

export default function useFormValidation() {
  const isFormValid = (data: { [key: string]: string }): boolean => {
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
      errors['passwordConfirmation'] =
        'Password and password confirmation must match.';
    }
  };

  return {
    errors,
    isFormValid,
    validateEmptyField,
    validatePassword,
    validatePasswordConfirmation,
  };
}
