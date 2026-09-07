using System.Text.Json.Serialization;
using Authorization;
using IdentityProvider.EndpointDTOs;
using IdentityProvider.Endpoints;
using Utilities;

namespace Data;

public class UserManager
{
     //      private IndexFile? IndexFile { get; set; }
     //      private List<IndexEntry> UserIndexes { get; set; } = [];
     //      private BlobService BlobService { get; set; }
     //      public UserManager(BlobService blobService)
     //      {
     //           BlobService = blobService;
     //      }

     //      public static async Task<UserManager> Open(BlobService blobService)
     //      {
     //           UserManager userManager = new(blobService)
     //           {
     //                IndexFile = await IndexFile.RetrieveAsync(blobService)
     //           };
     //           userManager.UserIndexes = userManager.IndexFile.UserList;
     //           return userManager;
     //      }

     // public async Task<UserProfile> GetValidatedProfile(string accessName, string password, BlobService blobService)
     // {
     //      UserProfile? profile = await UserProfile.RetrieveAsync(accessName, blobService);
     //      if (profile is null) throw new Exception("NotFound");
     //      bool isVerified = PasswordCrypto.VerifyPassword(password, profile.HashedPassword, profile.Salt);
     //      if (isVerified is false) throw new Exception("Invalid credentials");
     //      return profile;
     // }
     // private IndexEntry FindIndex(string userName)
     // {
     //      IndexEntry? entry = UserIndexes.Find(u => u.UserName == userName);
     //      if (entry is null) throw new Exception("Index not found");
     //      return entry;
     // }
     // public async Task<bool> AddProfileAsync(AuthProfileDTO authProfile)
     // {
     //      if (BlobService is null || UserIndexes is null) return false;
     //      if (UserExists(authProfile.UserName)) throw new Exception("User already exists");
     //      UserProfile userProfile = UserProfileFactory.CreateUserProfile(authProfile);
     //      IndexEntry indexEntry = UserProfileFactory.CreateIndexEntry(userProfile);
     //      UserIndexes.Add(indexEntry);
     //      try
     //      {
     //           await userProfile.SaveAsync(BlobService);
     //      }
     //      catch { return false; }
     //      try
     //      {
     //           await IndexFile!.SaveAsync(BlobService);
     //      }
     //      catch
     //      {
     //           UserIndexes.RemoveAt(UserIndexes.Count - 1);
     //           await UserProfile.RemoveAsync(userProfile.Id, BlobService);
     //           return false;
     //      }
     //      return true;
     // }




     // public async Task<UserProfile?> GetProfileByUserName(string accessName)
     // {
     //      if (BlobService is null || UserIndexes is null) return null;
     //      IndexEntry? entry = UserIndexes.Find(u => u.UserName == accessName);
     //      if (entry is null) return null;
     //      return await UserProfile.RetrieveAsync(entry.Id, BlobService);
     // }

     // public async Task<UserProfile> GetProfileById(Guid id)
     // {
     //      return await UserProfile.RetrieveAsync(id, BlobService);
     // }

     // public async Task<UserProfile?> GetProfile(string id)
     // {
     //      if (BlobService is null || UserList is null) return null;
     //      return await UserProfile.RetrieveAsync(id, BlobService);
     // }
     // public IndexEntry FindIndex(string accessString)
     // {
     //      IndexEntry? profile = UserList.Find(u => u.UserName == accessString);
     //      if (profile is null) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, accessString);
     //      return profile;
     // }

}
// public class IndexFile : IBlobObject
// {
//      public List<IndexEntry> UserList { get; set; } = [];

//      //=================

//      public static string FullFileName() { return "index" + BlobNaming.Json; }
//      //   public string BlobPath() { return BlobService.CreateBlobPath(Folders.IDP, null, FullFileName()); }
//      public static string BlobPath() { return BlobService.CreateBlobPath(Folders.Users, null, FullFileName()); }
//      public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }

//      public static async Task<bool> ExistsAsync(BlobService blobService)
//      {
//           try
//           {
//                return await blobService.IsExisting(BlobPath());
//           }
//           catch { return false; }
//      }
//      public Task SaveAsync(BlobService blobService)
//      {
//           BlobDescriptor descriptor = new(BlobPath(), this);
//           return blobService.SaveAsync(descriptor);
//      }
//      public static async Task<IndexFile> RetrieveAsync(BlobService blobService)
//      {
//           return await blobService.RetrieveAsync<IndexFile>(BlobPath());
//      }
//      public void Validate() { }
// }
// public class IndexEntry
// {
//      public required string UserName { get; set; }
//      public Guid Id { get; set; }
//      [JsonConstructor] public IndexEntry() { }

// }