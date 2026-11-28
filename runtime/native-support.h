// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#ifndef XAMARIN_NATIVE_SUPPORT_H
#define XAMARIN_NATIVE_SUPPORT_H

#include <errno.h>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

static inline int
xamarin_write_all (int descriptor, const char *data, size_t length)
{
	while (length > 0) {
		ssize_t written = write (descriptor, data, length);
		if (written < 0 && errno == EINTR)
			continue;
		if (written <= 0) {
			if (written == 0)
				errno = EIO;
			return -1;
		}
		data += written;
		length -= (size_t) written;
	}
	return 0;
}

static inline int xamarin_write_formatted (int descriptor, const char *format, ...) __attribute__((format(__printf__, 2, 3)));

static inline int
xamarin_write_formatted (int descriptor, const char *format, ...)
{
	va_list args;
	va_start (args, format);
	char *message = NULL;
	int length = vasprintf (&message, format, args);
	va_end (args);
	if (length < 0) {
		int saved_errno = errno;
		const char failure [] = "Could not format native diagnostic.\n";
		xamarin_write_all (STDERR_FILENO, failure, sizeof (failure) - 1);
		errno = saved_errno;
		return -1;
	}
	FILE *stream = descriptor == STDOUT_FILENO ? stdout : (descriptor == STDERR_FILENO ? stderr : NULL);
	if (stream != NULL)
		flockfile (stream);
	int result = stream != NULL && fflush (stream) != 0 ? -1 : xamarin_write_all (descriptor, message, (size_t) length);
	int saved_errno = errno;
	if (stream != NULL)
		funlockfile (stream);
	free (message);
	if (result != 0) {
		const char failure [] = "Could not write native diagnostic.\n";
		xamarin_write_all (STDERR_FILENO, failure, sizeof (failure) - 1);
	}
	errno = saved_errno;
	return result == 0 ? length : -1;
}

struct XamarinErrorDescription {
	char text [256];
};

static inline struct XamarinErrorDescription
xamarin_error_description (int error)
{
	struct XamarinErrorDescription description = { { 0 } };
	int saved_errno = errno;
	int result = strerror_r (error, description.text, sizeof (description.text));
	if (result != 0) {
		strlcpy (description.text, "Error ", sizeof (description.text));
		size_t offset = sizeof ("Error ") - 1;
		unsigned int value = error < 0 ? 0u - (unsigned int) error : (unsigned int) error;
		if (error < 0)
			description.text [offset++] = '-';
		char digits [sizeof (value) * 3];
		size_t count = 0;
		do {
			digits [count++] = (char) ('0' + value % 10);
			value /= 10;
		} while (value != 0);
		while (count > 0)
			description.text [offset++] = digits [--count];
		description.text [offset] = 0;
		strlcat (description.text, " (description unavailable)", sizeof (description.text));
	}
	errno = saved_errno;
	return description;
}

#endif
