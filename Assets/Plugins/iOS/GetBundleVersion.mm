extern "C" {
    const char * _GetCFBundleVersion() {
        NSString *version = [[NSBundle mainBundle] bundleIdentifier];
        return strdup([version UTF8String]);
    }
}