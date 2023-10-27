function formatDate(date: string | null): string {
  if (!date) {
    return '';
  } else {
    const formattedTime = new Date(date).toLocaleTimeString([], {
      hour12: false,
    });
    const formattedDate = new Date(date)
      .toLocaleDateString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
      })
      .replace(/\//g, '/');

    return `${formattedDate}, ${formattedTime}`;
  }
}

export { formatDate };
