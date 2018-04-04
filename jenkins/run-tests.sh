#!/bin/bash -e

report_error ()
{
	printf "🔥 [Test run failed]($BUILD_URL/Test_Report/) 🔥\\n" >> $WORKSPACE/jenkins/pr-comments.md

	if test -f $WORKSPACE/tests/TestSummary.md; then
		printf "\\n" >> $WORKSPACE/jenkins/pr-comments.md
		cat $WORKSPACE/tests/TestSummary.md >> $WORKSPACE/jenkins/pr-comments.md
	fi
}
trap report_error ERR

export BUILD_REVISION=jenkins
cd $WORKSPACE
# Unlock
security default-keychain -s builder.keychain
security list-keychains -s builder.keychain
echo "Unlock keychain"
security unlock-keychain -p `cat ~/.config/keychain`
echo "Increase keychain unlock timeout"
security set-keychain-settings -lut 7200

# Run tests
make -C tests jenkins

printf "✅ [Test run succeeded]($BUILD_URL/Test_Report/)\\n" >> $WORKSPACE/jenkins/pr-comments.md
