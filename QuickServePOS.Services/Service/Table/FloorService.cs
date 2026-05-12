using AutoMapper;
using Org.BouncyCastle.Crypto;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Floor;
using QuickServePOS.Models.Entities.Table;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service
{
    public class FloorService : IFloorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FloorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<FloorListDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Floors.GetAllAsync();

            return _mapper.Map<List<FloorListDto>>(entities);
        }
       
        public async Task<FloorUpdateDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Floors.GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<FloorUpdateDto>(entity);
        }

        public async Task<ApiResponse> CreateAsync(FloorCreateDto dto)
        {
            var exists = await _unitOfWork.Floors.ExistsAsync(dto.Name);

            if (exists)
            {
                return Fail("Floor already exists");
            }

            var entity = _mapper.Map<FloorEntity>(dto);

            await _unitOfWork.Floors.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Floor created successfully");
        }

        public async Task<ApiResponse> UpdateAsync(FloorUpdateDto dto)
        {
            var entity = await _unitOfWork.Floors.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return Fail("Floor not found");
            }

            var exists = await _unitOfWork.Floors.ExistsAsync(dto.Name, dto.Id);

            if (exists)
            {
                return Fail("Floor already exists");
            }

            _mapper.Map(dto, entity);

            _unitOfWork.Floors.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Floor updated successfully");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Floors.GetByIdAsync(id);

            if (entity == null)
            {
                return Fail("Floor not found");
            }

            _unitOfWork.Floors.Delete(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Floor deleted successfully");
        }

        public async Task<ApiResponse> RestoreAsync(int id)
        {
            var entity = await _unitOfWork.Floors.GetByIdIgnoreQueryFilterAsync(id);

            if (entity == null)
            {
                return Fail("Floor not found");
            }

            entity.IsDeleted = false;
            entity.DeletedAt = null;

            _unitOfWork.Floors.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Floor restored successfully");
        }

        public async Task<List<FloorListDto>> GetDeletedAsync()
        {
            var entities = await _unitOfWork.Floors.GetDeletedFloorsAsync();

            return _mapper.Map<List<FloorListDto>>(entities);
        }

        private ApiResponse Success(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        private ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }

    }
}
