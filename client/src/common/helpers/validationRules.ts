function requiredField(value: string | number, name: string) {
  return !!value || `${name} is required`;
}

function passwordPattern(value: string) {
  const pattern = /^(?=.*[A-Z])(?=.*\W).{8,}$/;
  return (
    pattern.test(value) ||
    'Password must contain at least 8 characters including one non alphanumeric and one upper case character.'
  );
}

function validatePositive(value: number, name: string) {
  return value > 0 || `${name} must be positive`;
}

function validateInteger(value: number, name: string) {
  return Number.isInteger(value) || `${name} must be an integer value`;
}

export { requiredField, passwordPattern, validatePositive, validateInteger };
