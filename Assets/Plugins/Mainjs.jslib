 mergeInto(LibraryManager.library, {
  sendGateID:function(methodType,json) 
   {
       sendGateID(UTF8ToString(methodType),UTF8ToString(json));
   },
 });