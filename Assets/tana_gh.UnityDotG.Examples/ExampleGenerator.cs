using tana_gh.UnityDotG.Editor;

namespace tana_gh.UnityDotG.Examples
{
    [CodeGen]
    internal static class ExampleGenerator
    {
        public static void Generate(CodeGenContext context)
        {
            context.AddCode("Example.g.cs",
$@"
using UnityEngine;

namespace tana_gh.UnityDotG.Examples
{{
    public class ExampleBehaviour : MonoBehaviour
    {{
        private void Start()
        {{
            Debug.Log(""This message is auto-generated with type {typeof(ExampleGenerator).FullName}."");
        }}
    }}
}}
"
            );
        }
    }
}
