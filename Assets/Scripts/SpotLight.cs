using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public PlayerController playerCnt; //PlayerController�R���|�[�l���g
    public float rotationSpeed = 20.0f; //�X�|�b�g���C�g�̉�]���x
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //���O�܂ł̃X�|�b�g���̉�]�l(Z���̂ݎ擾)
        //float currentAngle = transform.eulerAngles.z;

        //�v���C���[�̊p�x
        float targetAngle = playerCnt.angleZ;

        //�^�[�Q�b�g�ƂȂ�p�x�𒲐�
        Quaternion targetRotation = Quaternion.Euler(0, 0, (targetAngle - 90));

        //���݂̉�]��(���O��])��(�^�[�Q�b�g�̉�])�ɂȂ�悤�ɂȂ߂炩�ɕ⊮���� Quaternion.Slerp���\�b�h
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}
