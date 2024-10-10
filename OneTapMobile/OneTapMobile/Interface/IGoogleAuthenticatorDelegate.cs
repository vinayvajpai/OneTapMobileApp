using System;
using System.Collections.Generic;
using System.Text;
using OneTapMobile.Authentication;

namespace OneTapMobile.Interface
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(GoogleOAuthToken token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}
