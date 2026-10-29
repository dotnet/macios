// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#ifndef XAMARIN_NATIVE_SUPPORT_H
#define XAMARIN_NATIVE_SUPPORT_H

#include <errno.h>
#include <string.h>

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
